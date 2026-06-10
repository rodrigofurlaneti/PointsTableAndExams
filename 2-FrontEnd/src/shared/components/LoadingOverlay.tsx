import { useEffect, useState } from 'react';
import { Spinner } from './Spinner';
import styles from './LoadingOverlay.module.css';

const WORD = 'Loading';
const TOTAL_STEPS = WORD.length + 3; // "Loading" + 3 dots

export function LoadingOverlay() {
  const [step, setStep] = useState(0);

  useEffect(() => {
    const id = setInterval(() => {
      setStep((s) => (s + 1) % (TOTAL_STEPS + 1));
    }, 500);
    return () => clearInterval(id);
  }, []);

  const letters = WORD.slice(0, Math.min(step, WORD.length));
  const dots = step > WORD.length ? '.'.repeat(step - WORD.length) : '';

  return (
    <div className={styles.overlay} role="status" aria-label="Loading">
      <div className={styles.content}>
        <Spinner size="lg" />
        <p className={styles.label}>
          {letters}
          <span className={styles.dots}>{dots}</span>
        </p>
      </div>
    </div>
  );
}
