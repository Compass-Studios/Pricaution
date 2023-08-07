"use client"
import {Box, Container, Typography} from "@mui/material";
import Header from "./components/header/Header";
import Searcher from "./components/main/Searcher";
import Sort from "./components/main/Sort";

export default function Home() {
  return (
    <Box>
        <Header />
        <Container sx={{display: "flex", justifyContent:"flex-start", marginTop: "5vh"}}>
            <Typography variant="h4" sx={{ color: "#4B37C0"}}>
                Be precautious with&nbsp;
            </Typography>
            <Typography variant="h4" sx={{ color: "#EC9A14"}}>
                your purchase
            </Typography>
        </Container>
        <Searcher />
        <Sort />
    </Box>
  )
}