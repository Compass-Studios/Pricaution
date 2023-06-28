# Data Merger
A small program that reads JSON files with offers, and merges the needed attributes into a CSV file for the ML model

## Usage
Prepare a directory with the input JSON files, following the example:
```
.
└── input-data
    ├── 1.json
    └── 2.json
```
The names can be arbitrary. Next, run the program with a path to the folder as a parameter:

    $ data-merger input-data

It will run through the files and produce a CSV file `data.csv` in the current working directory.

## Building
1. Make sure you have Rust installed, preferably via [rustup](https://rustup.rs)
2. Clone this repository, and navigate this project's root
3. Run `cargo build --release` to build a release-optimized binary. Afterward, you'll find it in `target/release/`
4. Alternatively, you can run `cargo install --path .` to automatically build and install it on your system.
