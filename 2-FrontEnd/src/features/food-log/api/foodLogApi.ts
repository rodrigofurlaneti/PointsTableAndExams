import { apiClient } from '../../../core/api/client';
import { useAuthStore } from '../../../core/auth/authStore';
import type { DailyLog, AddLogItemRequest } from '../types/foodLog.types';

const today = () => new Date().toISOString().split('T')[0]; // YYYY-MM-DD

export const foodLogApi = {
  /** Get today's log for the current user. Returns null if not yet created. */
  getTodayLog: async (): Promise<DailyLog | null> => {
    const userId = useAuthStore.getState().user?.id;
    if (!userId) return null;
    try {
      return await apiClient
        .get<DailyLog>(`/daily-logs/${userId}/${today()}`)
        .then(r => r.data);
    } catch (err: any) {
      if (err?.response?.status === 404) return null;
      throw err;
    }
  },

  /** Create today's log if it doesn't exist yet. Returns the log. */
  ensureTodayLog: async (): Promise<DailyLog> => {
    const existing = await foodLogApi.getTodayLog();
    if (existing) return existing;

    const userId = useAuthStore.getState().user?.id;
    const res = await apiClient.post<{ id: string }>('/daily-logs', {
      userId,
      logDate: today(),
      notes: null,
    });
    // Fetch the newly created log
    return await apiClient
      .get<DailyLog>(`/daily-logs/${userId}/${today()}`)
      .then(r => r.data);
  },

  getHistory: (userId: string, from: string, to: string) =>
    apiClient
      .get(`/daily-logs/${userId}/history`, { params: { from, to } })
      .then(r => r.data),

  addItem: (logId: string, data: AddLogItemRequest) =>
    apiClient
      .post<{ id: string }>(`/daily-logs/${logId}/items`, data)
      .then(r => r.data),

  getFoodItems: (search?: string) =>
    apiClient
      .get('/food-items', { params: search ? { search } : undefined })
      .then(r => r.data),
};
