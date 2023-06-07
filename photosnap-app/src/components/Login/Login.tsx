import { Alert, AlertTitle, Button, Card, Container, CssBaseline, Divider, Paper, TextField, Typography } from "@mui/material";
import { Box } from "@mui/system";
import axios from "axios";
import { Fragment, useEffect, useState } from "react";
import { Navigate, Link, useLocation, useNavigate } from "react-router-dom";
import IUserLoggedStatusChange from "../../Interfaces/LoginAndRegisterInterfaces/IUserLogged";
import LoginRegisterResponse from "../../Types/LoginAndRegisterTypes/LoginRegisterResponse";

function Login(prop: IUserLoggedStatusChange) {
    let navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [invalidCredentialsFlag, setInvalidCredentialsFlag] = useState(false);
    const [isLoginSuccesful, setIsLoginSuccesful] = useState(false);


    const handleUsernameChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setUsername(e.currentTarget.value);
    }

    const handlePasswordChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setPassword(e.currentTarget.value);
    }

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (username.length === 0 && password.length === 0)
            setInvalidCredentialsFlag(true);
        else {
            setInvalidCredentialsFlag(false);
            const formData = new FormData();
            formData.append("username", username);
            formData.append("password", password);
            axios.post<LoginRegisterResponse>('https://localhost:7053/api/User/Login', formData)
                .then((response) => {
                    if (response.status === 200) {
                        setInvalidCredentialsFlag(false);
                        window.sessionStorage.setItem("token", response.data.token);
                        window.sessionStorage.setItem("username", response.data.username);
                        window.sessionStorage.setItem("profilePhoto", response.data.profilePhoto);
                        window.sessionStorage.setItem("isUserLogged", "true");

                        setIsLoginSuccesful(true);
                        prop.userLogged();
                    }
                })
                .catch((err) => { setInvalidCredentialsFlag(true); });
        }
    };

    useEffect(() => {
        document.title = "Login";
    }, []);

    useEffect(() => {
        document.title = `Login`;
    },[])

    return (
        <Container >
            {isLoginSuccesful ? <Navigate to={"/homepage"} /> : ""}
            <CssBaseline />
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}

            >
                <Typography variant="h2">Welcome back on Photosnap</Typography>

                <Box component="form" onSubmit={handleSubmit} noValidate sx={{ textAlign: 'center' }} >
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="username"
                        label="Username"
                        name="username"
                        autoComplete="Username"
                        autoFocus
                        onChange={handleUsernameChange}
                        color={username.length ? 'primary' : 'error'}
                    />

                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="password"
                        label="Password"
                        name="password"
                        type="password"
                        autoComplete="current-password"
                        value={password}
                        onChange={handlePasswordChange}
                        color={password.length ? 'primary' : 'error'}
                    />


                    {invalidCredentialsFlag === true ?
                        <Alert severity="warning">
                            <AlertTitle >Upozorenje</AlertTitle>
                            User with given username and password does not exist — <strong>check for mispeling!</strong>
                        </Alert> : ""}


                    <Button
                        type="submit"
                        size="medium"
                        variant="contained"
                        sx={{
                            mt: 3, mb: 2, background: '#BA1B2A', ':hover': {
                                bgcolor: '#E65664',
                                color: 'FFFFFF',
                            },
                        }}
                    >
                        Login
                    </Button>

                </Box>
            </Box>

            <Box sx={{
                marginTop: 4,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center'
            }}>
                <Divider variant="fullWidth" flexItem />
                <Typography variant="h5"> Don't have an account? Register now!</Typography>
                <Link to='/Register' style={{ textDecoration: 'none' }}>
                    <Button
                        type="submit"
                        size="medium"
                        variant="contained"
                        sx={{
                            mt: 3, mb: 2, textAlign: 'center', background: '#BA1B2A', ':hover': {
                                bgcolor: '#E65664',
                                color: 'FFFFFF',
                            },
                        }}
                    >
                        Register
                    </Button>
                </Link>
            </Box>
        </Container>
    );
}
export default Login;