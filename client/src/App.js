import './App.css';
import Donors from './Donors';
import Login from './Login';
import Presents from './Presents';
import Register from './Register'
import PresentsManagement from './PresentsManagement';
import MenuBarUser from './menuBarUser'
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import PurchasesManagement from './PurchasesManagement';

export default function App(){
  return (
    <>

<BrowserRouter>
    <Routes>
      <Route path="/" element={<Login />} /> 
      <Route path="/Register" element={<Register />} />
      <Route path="/Presents" element={<Presents />} />
      <Route path="/PresentsManagement" element={<PresentsManagement />} />
      <Route path="/PurchasesManagement" element={<PurchasesManagement />} />
      <Route path="/Donors" element={<Donors/>} />
      <Route path="/MenuBarUser" element={<MenuBarUser/>} />

    </ Routes>
  </BrowserRouter>
  
    
    </>
  );
}

