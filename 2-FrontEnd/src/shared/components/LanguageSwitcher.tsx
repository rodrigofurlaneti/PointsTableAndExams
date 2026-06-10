import { useTranslation } from 'react-i18next';
import { SUPPORTED_LANGS } from '../../i18n';
import styles from './LanguageSwitcher.module.css';

export function LanguageSwitcher() {
  const { i18n } = useTranslation();
  const current = i18n.language;

  return (
    <div className={styles.switcher} role="group" aria-label="Language">
      {SUPPORTED_LANGS.map(({ code, label, flag }) => (
        <button
          key={code}
          className={`${styles.btn} ${current === code ? styles.active : ''}`}
          onClick={() => i18n.changeLanguage(code)}
          aria-pressed={current === code}
          aria-label={`Switch to ${label}`}
        >
          <span className={styles.flag}>{flag}</span>
          {label}
        </button>
      ))}
    </div>
  );
}
