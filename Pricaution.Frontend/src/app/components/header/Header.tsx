import {AppBar, Container, Box, Button, Typography, IconButton} from "@mui/material";
import Image from "next/image";
import DarkMode from "@mui/icons-material/DarkMode";
import LightMode from "@mui/icons-material/LightMode";

const pages = ['Home Page','About us','Contact'];
let darkMode = false;

const darkModeChange = () => {
    !darkMode
}

export default function Header() {

    return(
        <AppBar position="static" sx={{ background: "#FFF", width: "100vw"}}>
            <Container sx={{ margin: "0px"}}>
                <Box sx={{ display: { xs: 'none', sm: 'block' } }}>
                    <Typography component="a" href="/" sx={{textDecoration: 'none'}}>
                        <Image alt="Logo Pricaution" src="" />
                    </Typography>
                    {pages.map((item) => (
                        <Button key={item}>
                            <Typography key="href" component="a" href="/" sx={{ color: '#000', fontSize: "20px"}}>
                                {item}
                            </Typography>
                        </Button>
                    ))}
                    <IconButton size="large" onClick={darkModeChange}>
                        {darkMode? <DarkMode />:<LightMode />}
                    </IconButton>
                </Box>
            </Container>
        </AppBar>
    )
}