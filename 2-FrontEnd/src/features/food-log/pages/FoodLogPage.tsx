import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { SubNav } from '../../../design-system/components/Nav/SubNav';
import { Input } from '../../../design-system/components/Input/Input';
import { Button } from '../../../design-system/components/Button/Button';
import { Spinner } from '../../../shared/components/Spinner';
import { useTodayLog, useFoodItems, useAddLogItem } from '../hooks/useFoodLog';
import styles from './FoodLogPage.module.css';

const schema = z.object({
  foodItemId: z.string().min(1, 'Select a food item'),
  quantity:   z.coerce.number().min(0.5).max(20),
  mealTime:   z.string().optional(),
});

type FormData = z.infer<typeof schema>;

const MEAL_TIMES = ['07:00', '10:00', '12:00', '15:00', '19:00', '21:00'];

export default function FoodLogPage() {
  const [search, setSearch] = useState('');
  const { data: log, isLoading: logLoading } = useTodayLog();
  const { data: foodItems = [] } = useFoodItems(search || undefined);
  const { mutate: addItem, isPending: adding } = useAddLogItem();

  const { register, handleSubmit, reset, watch, formState: { errors } } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: { quantity: 1, mealTime: '12:00' },
  });

  const selectedItemId = watch('foodItemId');
  const selectedItem = foodItems.find(fi => fi.id === selectedItemId);

  const today = new Date().toLocaleDateString('en-US', {
    weekday: 'long', year: 'numeric', month: 'long', day: 'numeric',
  });

  const onSubmit = (data: FormData) => {
    if (!selectedItem) return;
    addItem(
      { foodItemId: data.foodItemId, quantity: data.quantity, pointsPerServing: selectedItem.points, mealTime: data.mealTime },
      { onSuccess: () => reset({ quantity: 1, mealTime: '12:00' }) }
    );
  };

  return (
    <div className={styles.page}>
      <SubNav category="Food Log" />

      <div className={styles.hero}>
        <h1 className={styles.title}>Daily Food Log</h1>
        <p className={styles.dateBar}>{today}</p>
        {log && (
          <div className={styles.pointsBadge}>
            <span className={styles.pointsNum}>{log.totalPoints}</span>
            <span className={styles.pointsDen}>/ 300 pts</span>
          </div>
        )}
      </div>

      <div className={styles.body}>
        {/* Add item panel */}
        <div className={styles.panel}>
          <p className={styles.panelTitle}>Add food item</p>
          <Input label="Search food" placeholder="e.g. Rice, Apple…" value={search} onChange={e => setSearch(e.target.value)} />

          <form onSubmit={handleSubmit(onSubmit)} noValidate style={{ display: 'flex', flexDirection: 'column', gap: 'var(--space-md)' }}>
            <div>
              <label style={{ fontSize: 'var(--text-caption-strong)', fontWeight: 600, color: 'var(--color-ink)', display: 'block', marginBottom: 4 }}>Food item</label>
              <select style={{ width: '100%', padding: '12px 20px', borderRadius: 'var(--radius-pill)', border: '1px solid rgba(0,0,0,0.08)', fontFamily: 'var(--font-body)', fontSize: 'var(--text-body)', color: 'var(--color-ink)', height: '44px', background: 'var(--color-canvas)' }} {...register('foodItemId')}>
                <option value="">Select…</option>
                {foodItems.map(fi => (
                  <option key={fi.id} value={fi.id}>{fi.name} — {fi.points}pts{fi.servingSize ? ` (${fi.servingSize})` : ''}</option>
                ))}
              </select>
              {errors.foodItemId && <p style={{ color: '#d70015', fontSize: 'var(--text-caption)', marginTop: 4 }}>{errors.foodItemId.message}</p>}
            </div>

            <Input label="Quantity" type="number" min={0.5} max={20} step={0.5} error={errors.quantity?.message} {...register('quantity')} />

            <div>
              <label style={{ fontSize: 'var(--text-caption-strong)', fontWeight: 600, color: 'var(--color-ink)', display: 'block', marginBottom: 4 }}>Meal time</label>
              <select style={{ width: '100%', padding: '12px 20px', borderRadius: 'var(--radius-pill)', border: '1px solid rgba(0,0,0,0.08)', fontFamily: 'var(--font-body)', fontSize: 'var(--text-body)', color: 'var(--color-ink)', height: '44px', background: 'var(--color-canvas)' }} {...register('mealTime')}>
                {MEAL_TIMES.map(t => <option key={t} value={t}>{t}</option>)}
              </select>
            </div>

            <Button type="submit" disabled={adding} style={{ width: '100%' }}>{adding ? 'Adding…' : 'Add item'}</Button>
          </form>
        </div>

        {/* Today's items */}
        <div className={styles.list}>
          <div className={styles.listHeader}>Today's items ({log?.items.length ?? 0})</div>
          {logLoading ? <Spinner /> : !log || log.items.length === 0 ? (
            <p className={styles.emptyState}>No items yet. Add your first meal above.</p>
          ) : (
            log.items.map(item => (
              <div key={item.id} className={styles.listItem}>
                <div style={{ flex: 1 }}>
                  <p className={styles.itemName}>{item.foodItemName}</p>
                  <p className={styles.itemMeta}>×{item.quantity}{item.mealTime ? ` · ${item.mealTime}` : ''}</p>
                </div>
                <span className={styles.itemPoints}>{item.pointsComputed}pt</span>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
}
