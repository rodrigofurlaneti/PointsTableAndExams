import { createBrowserRouter, RouterProvider, Outlet } from 'react-router-dom';
import { Suspense, lazy } from 'react';
import { ProtectedRoute } from '../auth/ProtectedRoute';
import { GlobalNav } from '../../design-system/components/Nav/GlobalNav';
import { Footer } from '../../design-system/components/Footer/Footer';
import { Spinner } from '../../shared/components/Spinner';

/* Lazy-load all pages for code splitting */
const LoginPage        = lazy(() => import('../../features/auth/pages/LoginPage'));
const RegisterPage     = lazy(() => import('../../features/auth/pages/RegisterPage'));
const DashboardPage    = lazy(() => import('../../features/dashboard/pages/DashboardPage'));
const FoodLogPage      = lazy(() => import('../../features/food-log/pages/FoodLogPage'));
const FoodLogHistory   = lazy(() => import('../../features/food-log/pages/FoodLogHistoryPage'));
const ExamsPage        = lazy(() => import('../../features/exams/pages/ExamsPage'));
const ExamRequestPage  = lazy(() => import('../../features/exams/pages/ExamRequestPage'));

/** Root layout: GlobalNav + content + Footer */
function RootLayout() {
  return (
    <>
      <GlobalNav />
      <main style={{ paddingTop: '44px' }}>
        <Suspense fallback={<Spinner fullPage />}>
          <Outlet />
        </Suspense>
      </main>
      <Footer />
    </>
  );
}

const router = createBrowserRouter([
  {
    element: <RootLayout />,
    children: [
      /* Public routes */
      { path: '/login',    element: <LoginPage /> },
      { path: '/register', element: <RegisterPage /> },

      /* Protected routes */
      {
        element: <ProtectedRoute />,
        children: [
          { path: '/',                  element: <DashboardPage /> },
          { path: '/dashboard',         element: <DashboardPage /> },
          { path: '/food-log',          element: <FoodLogPage /> },
          { path: '/food-log/history',  element: <FoodLogHistory /> },
          { path: '/exams',             element: <ExamsPage /> },
          { path: '/exams/requests',    element: <ExamRequestPage /> },
        ],
      },
    ],
  },
]);

export function AppRouter() {
  return <RouterProvider router={router} />;
}
