"use client"
import {Box, Container, Typography} from "@mui/material";
import Header from "./components/header/Header";
export default function Home() {
  return (
    <Box>
        <Header />
        <Container>
            <Typography variant="h3" sx={{ color: "#4B37C0"}}>
                Be precautious with your purchase
            </Typography>
        </Container>
    </Box>
  )
}