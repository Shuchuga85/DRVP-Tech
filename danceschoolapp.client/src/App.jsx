import { Routes, Route } from 'react-router-dom'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import AdminUsersPage from './pages/AdminUsersPage'

function App() {
    return (
        <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/admin" element={<AdminUsersPage />} />
        </Routes>
    )
}

export default App

//import AdminUsersPage from './pages/AdminUsersPage'

//function App() {
//    return <AdminUsersPage />
//}

//export default App