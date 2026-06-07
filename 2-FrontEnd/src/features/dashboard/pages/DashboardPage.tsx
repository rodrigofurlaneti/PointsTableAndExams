import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../../../core/auth/authStore';
import { useTodayLog } from '../../food-log/hooks/useFoodLog';
import { useMyExamRequests } from '../../exams/hooks/useExams';
import { Button } from '../../../design-system/components/Button/Button';
import { StoreCard } from '../../../design-system/components/Card/StoreCard';
import styles from './DashboardPage.module.css';

const DAILY_LIMIT = 300;

export default function DashboardPage() {
  const user = useAuthStore((s) => s.user);
  const navigate = useNavigate();
  const firstName = user?.fullName?.split(' ')[0] ?? 'there';

  const { data: todayLog } = useTodayLog();
  const { data: examRequests = [] } = useMyExamRequests();

  const todayPoints   = todayLog?.totalPoints ?? 0;
  const todayItems    = todayLog?.items?.length ?? 0;
  const pendingExams  = examRequests
    .flatMap((r) => r.items)
    .filter((i) => !i.isCompleted).length;
  const progressPct   = Math.min((todayPoints / DAILY_LIMIT) * 100, 100).toFixed(1);

  return (
    <div className={styles.page}>

      {/* ── Hero dark tile ───────────────────────────────── */}
      <section className={styles.hero}>
        <div className={styles.heroInner}>
          <p className={styles.greeting}>Good day, {firstName}.</p>
          <h1 className={styles.heroTitle}>Your Health Dashboard</h1>
          <p className={styles.heroSub}>Track what you eat. Stay on top of your exams.</p>
        </div>
      </section>

      {/* ── Stats ─────────────────────────────────────────── */}
      <section className={styles.statsSection}>
        <div className={styles.statsGrid}>
          <div className={styles.statCard}>
            <p className={styles.statLabel}>Today's Points</p>
            <p className={styles.statValue}>
              {todayPoints} <span className={styles.statUnit}>/ {DAILY_LIMIT}</span>
            </p>
            <div className={styles.progressBar}>
              <div className={styles.progressFill} style={{ width: `${progressPct}%` }} />
            </div>
          </div>
          <div className={styles.statCard}>
            <p className={styles.statLabel}>Food items today</p>
            <p className={styles.statValue}>{todayItems}</p>
          </div>
          <div className={styles.statCard}>
            <p className={styles.statLabel}>Pending exams</p>
            <p className={styles.statValue}>{pendingExams}</p>
          </div>
        </div>
      </section>

      {/* ── Quick actions (parchment tile) ────────────────── */}
      <section className={styles.actionsSection}>
        <div className={styles.actionsInner}>
          <h2 className={styles.sectionTitle}>What would you like to do?</h2>
          <div className={styles.cardsGrid}>
            <StoreCard
              name="Log today's food"
              meta="Record meals and track daily points"
              cta={<Button variant="secondary" onClick={() => navigate('/food-log')}>Open Food Log</Button>}
            />
            <StoreCard
              name="View exam requests"
              meta="Check pending and completed exams"
              cta={<Button variant="secondary" onClick={() => navigate('/exams')}>Go to Exams</Button>}
            />
            <StoreCard
              name="Points history"
              meta="Browse your daily points over time"
              cta={<Button variant="secondary" onClick={() => navigate('/food-log/history')}>View History</Button>}
            />
          </div>
        </div>
      </section>

    </div>
  );
}
