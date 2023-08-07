import {Container, Box, TextField, Button, Typography} from "@mui/material";

export default function Searcher() {

    return(
        <Container sx={{display: "flex",background: "#FFF", padding: "35px", marginTop: "5vh", marginBottom: "5vh", justifyContent: "space-between", flexWrap: "wrap"}}>
            <Box>
                <Typography>Location</Typography>
                <TextField id="outlined-search" label="Enter city" type="search" sx={{width: "25vw"}}/>
            </Box>
            <Box>
                <Typography>Surface (in m<sup>2</sup>)</Typography>
                <TextField id="outlined-search" label="from" type="number" sx={{width: "10vw", marginRight: "2vw"}}/>
                <TextField id="outlined-search" label="to" type="number" sx={{width: "10vw"}}/>
            </Box>
            <Box>
                <Typography>Price (in zł)</Typography>
                <TextField id="outlined-search" label="from" type="number" sx={{width: "10vw", marginRight: "2vw"}}/>
                <TextField id="outlined-search" label="to" type="number" sx={{width: "10vw"}}/>
            </Box>
            <Box>
                <Button sx={{color: "#EC9A14", marginTop: "3vh", padding: "2vh", marginRight: "2vw"}}>
                    <Typography>
                        Clear
                    </Typography>
                </Button>
                <Button sx={{background: "#EC9A14", color: "rgb(80, 78, 78)", marginTop: "3vh", padding: "2vh 3vh"}}>
                    <Typography>
                        Search
                    </Typography>
                </Button>
            </Box>
        </Container>
    )
}