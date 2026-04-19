import { Outlet } from 'react-router-dom'
import Navbar from '../pages/public/sections/Navbar'
function AppLayout({ simple }) {
    return (
        <>
            <Navbar simple={simple} />
            <Outlet />
        </>
    )
}

export default AppLayout