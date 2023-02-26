import React from 'react';
import { Route, Routes } from 'react-router-dom';

import './App.css';
import Header from './components/header/Header';

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Header />} />
      </Routes>
    </>
  );
}

export default App;
