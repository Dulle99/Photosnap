import { Container, Grid, Typography } from "@mui/material";
import { useEffect, useState } from "react";

function Homepage() {
    const [homepageText, setHomepageText] = useState("");
    useEffect(() => {
        const token = sessionStorage.getItem('token');
        if (token == null || token.length == 0)
            setHomepageText("Explore photosnap");
        else
            setHomepageText("By your favorite")
    })
    return (
        <>
            <Grid padding={1}>

                <Typography component="h1"
                    variant="h3"
                    align="left"
                    color="text.primary">{homepageText}
                </Typography>
                <Grid>
                    {/*recommendedCookingRecepies.map((cookingRecepie) => (<CookingRecepiePreview {...cookingRecepie} key={cookingRecepie.cookingRecepieId} />))*/}
                </Grid>
            </Grid>

        </>
    );
}

export default Homepage;