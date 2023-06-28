use std::env::args;
use std::process::exit;
use std::path::PathBuf;
use std::fs;
use serde::{Serialize, Deserialize};

#[derive(Debug, Deserialize, Serialize)]
#[serde(rename_all(deserialize = "PascalCase", serialize = "snake_case"))]
struct DataEntry {
    #[serde(skip_deserializing)]
    id: Option<usize>,
    address: String,
    city: String,
    floor: i16,
    price: u32,
    rooms: u16,
    sq: f32,
    year: Option<i32>,
}

fn main() {
    let input_dir = if let Some(arg) = args().nth(1) {
        let path = PathBuf::from(arg);
        if !path.is_dir() {
            eprintln!("The provided path is not a directory");
            exit(1);
        }
        path
    } else {
        eprintln!("Please provide a path to directory containing input files");
        exit(1);
    };

    let mut data_vec = Vec::<DataEntry>::new();
    for entry in input_dir.read_dir().expect("error reading input directory") {
        let entry = entry.unwrap();
        if entry.path().is_dir() { continue }
        if !entry.file_name().to_str().unwrap().ends_with(".json") { continue }

        let contents = fs::read_to_string(entry.path())
            .unwrap_or_else(|e| panic!("error reading file {:?}: {e}", entry.file_name()));
        let mut contents_parsed: Vec<DataEntry> = serde_json::from_str(&contents)
            .unwrap_or_else(|e| panic!("error parsing JSON in file {:?}: {e}", entry.file_name()));
        data_vec.append(&mut contents_parsed);
    }

    let mut wrt = csv::Writer::from_path("data.csv").unwrap();
    for (id, entry) in data_vec.into_iter().enumerate() {
        let mut entry = entry;
        entry.id = Some(id);
        wrt.serialize(entry).unwrap();
    }
    wrt.flush().expect("error writing CSV data");

    println!("Done!");
}
