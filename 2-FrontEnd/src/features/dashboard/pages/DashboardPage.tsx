import { Link } from 'react-router-dom';
import { useAuthStore } from '../../../core/auth/authStore';
import { useDashboard } from '../hooks/useDashboard';
import { Button } from '../../../design-system/components/Button/Button';
import { StoreCard } from '../../../design-system/components/Card/StoreCard';
import { Spinner } from '../../../shared/components/Spinner';
import styles from './DashboardPage.module.css';

export default function DashboardPage() {
  const user = useAuthStore((s) => s.user);
  const { data, isLoading } = useDashboard();

  const firstName = user?.fullName?.split(' ')[0] ?? 'there';
  const pointsPct = data ? Math.min((data.todayPoints / data.dailyLimit) * 100, 100) : 0;

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
        {isLoading ? (
          <Spinner />
        ) : (
          <div className={styles.statsGrid}>
            <div className={styles.statCard}>
              <p className={styles.statLabel}>Today's Points</p>
              <p className={styles.statValue}>
                {data?.todayPoints ?? 0}
                <span className={styles.statUnit}>/ {data?.dailyLimit ?? 300}</span>
              </p>
              <div className={styles.progressBar}>
                <div className={styles.progressFill} style={{ width: `${pointsPct}%` }} />
              </div>
            </div>

            <div className={styles.statCard}>
              <p className={styles.statLabel}>Food items today</p>
              <p className={styles.statValue}>{data?.todayItemCount ?? 0}</p>
            </div>

            <div className={styles.statCard}>
              <p className={styles.statLabel}>Pending exams</p>
              <p className={styles.statValue}>{data?.pendingExams ?? 0}</p>
            </div>
          </div>
        )}
      </section>

      {/* ── Quick actions (parchment tile) ────────────────── */}
      <section className={styles.actionsSection}>
        <div className={styles.actionsInner}>
          <h2 className={styles.sectionTitle}>What would you like to do?</h2>
          <div className={styles.cardsGrid}>
            <StoreCard
              name="Log today's food"
              meta="Record meals and track daily points"
              cta={
                <Button variant="secondary" asChild>
                  <Link to="/food-log">Open Food Log</Link>
                </Button>
              }
            />
            <StoreCard
              name="View exam requests"
              meta="Check pending and completed exams"
              cta={
                <Button variant="secondary" asChild>
                  <Link to="/exams">Go to Exams</Link>
                </Button>
              }
            />
            <StoreCard
              name="Points history"
              meta="Browse your daily points over time"
              cta={
                <Button variant="secondary" asChild>
                  <Link to="/food-log/history">View History</Link>
                </Button>
              }
            />
          </div>
        </div>
      </section>

    </div>
  );
}
