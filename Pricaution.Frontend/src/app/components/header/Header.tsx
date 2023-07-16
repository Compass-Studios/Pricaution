import {AppBar, Container, Box, Button, Typography} from "@mui/material";
import Image from "next/image";

const pages = ['Home Page','About us','Contact'];

export default function Header() {

    return(
        <AppBar position="static" sx={{ background: "#FFF", width: "100vw", marginBottom: "3vh"}}>
            <Container sx={{display: "flex", justifyContent: "space-between"}}>
                <Typography component="a" href="/">
                    <Image alt="Logo Pricaution" src="" />
                </Typography>
                <Box>
                    {pages.map((item) => (
                        <Button key={item}>
                            <Typography key="href" component="a" href="/" sx={{ color: 'rgb(80, 78, 78)', fontSize: "20px", margin: "1vh"}}>
                                {item}
                            </Typography>
                        </Button>
                    ))}
                </Box>
            </Container>
        </AppBar>
    )
}