import { Toolbar, Container, Box, Typography } from "@mui/material";
import { Fragment, useEffect, useState } from "react";
import { Link, Outlet } from 'react-router-dom';
import HeaderLoggedUserButtons from "./Header-LoggedUserButtons";
import HeaderNonLoggedUserButtons from "./Header-NonLoggedUserButtons";

function Header() {
    const [isUserLogged, setIsUserLogged] = useState(false);
    const [path, setPath] = useState("ExplorePhotosnap");
    useEffect(() => {
        const token = sessionStorage.getItem('token');
        if (token == null || token.length == 0){
            setIsUserLogged(false);
            setPath("ExplorePhotosnap")
        }
        else{
            setPath("homepage")
            setIsUserLogged(true);
        }
    }, [window.sessionStorage.getItem('isUserLogged')])

    return (
        <>
            <Toolbar
                sx={{
                    background: '#BA1B2A',
                    borderBottom: 1,
                    borderColor: 'divider',
                    display: "flex",
                    flexWrap: "wrap"
                }}
            >
                <Container
                    style={{
                        display: 'flex',
                        flex: 1,
                        flexDirection: "row",
                        flexWrap: 'wrap',
                        justifyContent: 'left',

                    }}
                >
                    <Link to={path} style={{ textDecoration: 'none' }}>
                        <Box sx={{ display: "flex" }} >

                            <Typography
                                align='center'
                                color='#FFFFFF'
                                component='h2'
                                variant='h4'
                                noWrap
                            >
                                {"Photosnap"}
                            </Typography>
                        </Box>
                    </Link>


                </Container>
                <Box sx={{ flexGrow: 1, display: { xs: 'flex', justifyContent: "right" } }}>
                    {isUserLogged == true ? <HeaderLoggedUserButtons /> : <HeaderNonLoggedUserButtons />}
                </Box>
            </Toolbar>
            <Outlet />

        </>
    );
}
export default Header;