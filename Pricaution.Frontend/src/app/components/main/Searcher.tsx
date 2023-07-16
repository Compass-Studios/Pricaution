import {Container, TextField, Button, Typography} from "@mui/material";

export default function Searcher() {

    return(
        <Container sx={{background: "#FFF"}}>
            <TextField id="outlined-search" label="Wpisz miasto" type="seacrch" />
            <Button sx={{background: "#EC9A14", color: "rgb(80, 78, 78)"}}>
                <Typography>
                    Search
                </Typography>
            </Button>
        </Container>
    )
}