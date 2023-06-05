import React, { useEffect, useState } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import './App.css';
import Header from './components/Header/Header';
import Homepage from './components/Homepage/Homepage';
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import SelfUserProfile from './components/UserProfile/SelfUserProfile';
import PhotoForm from './components/Photo/Form/PhotoForm';
import ExplorePhotos from './components/Homepage/ExplorePhotos';
import UserProfile from './components/UserProfile/UserProfile';

function App() {
  let navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);

  function userLogged() {
    setToken(sessionStorage.getItem('token'));
    navigate('/homepage', {replace: true});
  }

  useEffect(() => {
    let path = sessionStorage.getItem('token') == null ? "/ExplorePhotosnap" : "/homepage"; 
    if (window.location.pathname === '/') {
      navigate(path, { replace: true });
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
          <Route path='ExplorePhotosnap' element={<ExplorePhotos/>} />
          <Route path='MyProfile' element={<SelfUserProfile />} >
          </Route>
          <Route path='UserProfile' element={<UserProfile />}/>
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
