import { SubNav } from '../../../design-system/components/Nav/SubNav';
import { Spinner } from '../../../shared/components/Spinner';
import { useFoodLogHistory } from '../hooks/useFoodLog';
import styles from './FoodLogPage.module.css';

export default function FoodLogHistoryPage() {
  const { data = [], isLoading } = useFoodLogHistory();

  return (
    <div className={styles.page}>
      <SubNav category="Food Log" links={[{ label: 'Today', to: '/food-log' }, { label: 'History', to: '/food-log/history' }]} />

      <div className={styles.hero}>
        <h1 className={styles.title}>Points History</h1>
        <p className={styles.dateBar}>Your daily point consumption over time</p>
      </div>

      <div style={{ maxWidth: 980, margin: '0 auto', padding: 'var(--space-xl) var(--space-lg)' }}>
        {isLoading ? (
          <Spinner />
        ) : data.length === 0 ? (
          <p style={{ textAlign: 'center', color: 'var(--color-ink-muted-48)', padding: 'var(--space-xxl)' }}>
            No history yet.
          </p>
        ) : (
          <div className={styles.list}>
            <div className={styles.listHeader}>All logged days</div>
            {data.map((row) => (
              <div key={row.logDate} className={styles.listItem}>
                <div style={{ flex: 1 }}>
                  <p className={styles.itemName}>
                    {new Date(row.logDate).toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric' })}
                  </p>
                  <p className={styles.itemMeta}>{row.foodItemCount} food items</p>
                </div>
                <span className={styles.itemPoints}>{row.totalPoints}pts</span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
