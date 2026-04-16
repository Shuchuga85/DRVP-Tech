import { Routes, Route } from 'react-router-dom'
import DashboardLayout from '../layouts/DashboardLayout'
import AdminDashboardPage from '../pages/admin/AdminDashboardPage'
import AdminUsersPage from '../pages/admin/AdminUsersPage'
import DirecaoDashboardPage from '../pages/direcao/DirecaoDashboardPage'
import ProfessorDashboardPage from '../pages/professor/ProfessorDashboardPage'
import EncarregadoDashboardPage from '../pages/encarregado/EncarregadoDashboardPage'

function AppRoutes() {
    return (
        <Routes>
            <Route element={<DashboardLayout role="admin" />}>
                <Route path="/admin" element={<AdminDashboardPage />} />
                <Route path="/admin/utilizadores" element={<AdminUsersPage />} />
            </Route>

            <Route element={<DashboardLayout role="direcao" />}>
                <Route path="/direcao" element={<DirecaoDashboardPage />} />
            </Route>

            <Route element={<DashboardLayout role="professor" />}>
                <Route path="/professor" element={<ProfessorDashboardPage />} />
            </Route>

            <Route element={<DashboardLayout role="encarregado" />}>
                <Route path="/encarregado" element={<EncarregadoDashboardPage />} />
            </Route>
        </Routes>
    )
}

export default AppRoutes