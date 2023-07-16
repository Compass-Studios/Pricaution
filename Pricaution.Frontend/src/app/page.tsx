"use client"
import {Box, Container, Typography} from "@mui/material";
import Header from "./components/header/Header";
import Searcher from "./components/main/Searcher";
export default function Home() {
  return (
    <Box>
        <Header />
        <Container sx={{display: "flex", justifyContent:"flex-start"}}>
            <Typography variant="h4" sx={{ color: "#4B37C0"}}>
                Be precautious with&nbsp;
            </Typography>
            <Typography variant="h4" sx={{ color: "#EC9A14"}}>
                your purchase
            </Typography>
        </Container>
        <Searcher />
    </Box>
  )
}