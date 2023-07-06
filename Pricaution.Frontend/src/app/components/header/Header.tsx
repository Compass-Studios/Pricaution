import {AppBar, Container, Box,  Button, Typography} from "@mui/material";
import Image from "next/image";

const pages = ['Home Page','About us','Contact'];
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
                            <Typography component="a" href="/" sx={{ color: '#000', fontSize: "20px"}}>
                                {item}
                            </Typography>
                        </Button>
                    ))}
                </Box>
            </Container>
        </AppBar>
    )
}