"use client"
import {Container, Typography} from "@mui/material";
import Header from "./components/header/Header";
export default function Home() {
  return (
    <Container>
        <Header />
      <Typography variant="h1">
          Be precautious with your purchase
      </Typography>
    </Container>
  )
}