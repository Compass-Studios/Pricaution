import * as React from 'react';
import {FormControl, Container, Select, MenuItem} from "@mui/material";

export default function Sort(){
    return(
        <Container>
            <FormControl variant="standard">
                <Select defaultValue={1}>
                    <MenuItem value={1}>Sort by the best</MenuItem>
                    <MenuItem value={2}>Sort by prices descending</MenuItem>
                    <MenuItem value={3}>Sort by prices ascending</MenuItem>
                    <MenuItem value={4}>Sort by cities A-Z</MenuItem>
                </Select>
            </FormControl>
        </Container>
    )
}