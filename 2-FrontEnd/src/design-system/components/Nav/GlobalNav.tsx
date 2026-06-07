import { Link, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../../../core/auth/authStore';
import { Button } from '../Button/Button';
import styles from './GlobalNav.module.css';

interface NavLink {
  label: string;
  to: string;
}

const NAV_LINKS: NavLink[] = [
  { label: 'Dashboard', to: '/dashboard' },
  { label: 'Food Log',  to: '/food-log' },
  { label: 'Exams',    to: '/exams' },
];

export function GlobalNav() {
  const { isAuthenticated, user, logout } = useAuthStore();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className={styles.nav} aria-label="Global navigation">
      <div className={styles.inner}>
        <Link to="/" className={styles.logo} aria-label="VitaLog home">
          Vita<span className={styles.logoAccent}>Log</span>
        </Link>

        {isAuthenticated && (
          <ul className={styles.links} role="list">
            {NAV_LINKS.map(({ label, to }) => (
              <li key={to}>
                <Link to={to} className={styles.link}>{label}</Link>
              </li>
            ))}
          </ul>
        )}

        <div className={styles.actions}>
          {isAuthenticated ? (
            <>
              <span className={styles.link} style={{ color: 'rgba(255,255,255,0.6)' }}>
                {user?.fullName?.split(' ')[0]}
              </span>
              <Button variant="dark" onClick={handleLogout}>
                Sign Out
              </Button>
            </>
          ) : (
            <Button variant="dark" onClick={() => navigate('/login')}>
              Sign In
            </Button>
          )}
        </div>
      </div>
    </nav>
  );
}
