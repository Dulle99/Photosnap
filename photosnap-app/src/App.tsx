import React, { useEffect, useState } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import './App.css';
import Header from './components/Header/Header';
import Homepage from './components/Homepage/Homepage';
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import SelfUserProfile from './components/UserProfile/SelfUserProfile';
import PhotoForm from './components/Photo/Form/PhotoForm';

function App() {
  let navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);

  function userLogged() {
    setToken(sessionStorage.getItem('token'));
  }

  useEffect(() => {
    if (window.location.pathname === '/') {
      navigate("/homepage", { replace: true });
    }
  }, []);

  useEffect(() => {
    setToken(sessionStorage.getItem('token'));
  }, [token]);

  return (
    <>
      <Routes>
        <Route path='/' element={<Header />}>
          <Route path='homepage' element={<Homepage />} />
          <Route path='MyProfile' element={<SelfUserProfile />} >
          </Route>
            <Route path='EditPhoto/:photoId' element={<PhotoForm isEditForm={true} />} />
          <Route path='PostPhoto' element={<PhotoForm isEditForm={false}  />} />
        </Route>
        <Route path='Login' element={<Login userLogged={userLogged} />} />
        <Route path='Register' element={<Register userLogged={userLogged} />} />

      </Routes>
    </>
  );
}

export default App;
