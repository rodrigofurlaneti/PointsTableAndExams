import { Link, useNavigate } from 'react-router-dom';
import { SubNav } from '../../../design-system/components/Nav/SubNav';
import { Button } from '../../../design-system/components/Button/Button';
import { Spinner } from '../../../shared/components/Spinner';
import { useMyExamRequests, useUpdateExamItem } from '../hooks/useExams';
import styles from './ExamsPage.module.css';

export default function ExamsPage() {
  const { data: requests = [], isLoading } = useMyExamRequests();
  const { mutate: updateItem } = useUpdateExamItem();
  const navigate = useNavigate();

  const pendingCount = requests
    .flatMap((r) => r.items)
    .filter((i) => !i.isCompleted).length;

  const markDone = (requestId: string, itemId: string) => {
    updateItem({
      requestId,
      itemId,
      data: { completedDate: new Date().toISOString().split('T')[0] },
    });
  };

  return (
    <div className={styles.page}>
      <SubNav
        category="Exams"
        links={[{ label: 'My requests', to: '/exams' }, { label: 'New request', to: '/exams/requests' }]}
      />

      <div className={styles.hero}>
        <h1 className={styles.title}>Exam Requests</h1>
        <p className={styles.subtitle}>
          {pendingCount > 0
            ? `You have ${pendingCount} pending exam${pendingCount > 1 ? 's' : ''}`
            : 'All exams completed'}
        </p>
      </div>

      <div className={styles.body}>
        <div>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 'var(--space-lg)' }}>
            <h2 className={styles.sectionTitle} style={{ marginBottom: 0 }}>My requests</h2>
            <Button variant="secondary" onClick={() => navigate('/exams/requests')}>+ New request</Button>
          </div>

          {isLoading ? (
            <Spinner />
          ) : requests.length === 0 ? (
            <p style={{ textAlign: 'center', color: 'var(--color-ink-muted-48)', padding: 'var(--space-xxl)' }}>
              No exam requests yet.{' '}
              <Link to="/exams/requests" style={{ color: 'var(--color-primary)' }}>Create one</Link>
            </p>
          ) : (
            <div style={{ display: 'flex', flexDirection: 'column', gap: 'var(--space-lg)' }}>
              {requests.map((req) => (
                <div key={req.id} className={styles.requestCard}>
                  <div className={styles.requestHeader}>
                    <div>
                      <p className={styles.requestDoctor}>{req.doctorName}</p>
                      <p className={styles.requestMeta}>
                        {new Date(req.requestDate).toLocaleDateString('en-US', { month: 'long', day: 'numeric', year: 'numeric' })}
                        {req.notes && ` · ${req.notes}`}
                      </p>
                    </div>
                    <p className={styles.requestMeta}>
                      {req.items.filter((i) => i.isCompleted).length}/{req.items.length} done
                    </p>
                  </div>

                  {req.items.map((item) => (
                    <div key={item.id} className={styles.examRow}>
                      <div style={{ flex: 1 }}>
                        <p className={styles.examName}>
                          {item.examName}
                          {item.abbreviation && <span style={{ color: 'var(--color-ink-muted-48)', marginLeft: 6 }}>({item.abbreviation})</span>}
                        </p>
                        <p className={styles.examCategory}>{item.examCategory}</p>
                        {item.result && <p style={{ fontSize: 'var(--text-caption)', color: 'var(--color-ink-muted-80)', marginTop: 2 }}>Result: {item.result}</p>}
                      </div>

                      <span className={`${styles.badge} ${item.isCompleted ? styles.done : styles.pending}`}>
                        {item.isCompleted ? 'Done' : 'Pending'}
                      </span>

                      {!item.isCompleted && (
                        <Button
                          variant="pearl"
                          onClick={() => markDone(req.id, item.id)}
                          aria-label={`Mark ${item.examName} as done`}
                        >
                          Mark done
                        </Button>
                      )}
                    </div>
                  ))}
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
