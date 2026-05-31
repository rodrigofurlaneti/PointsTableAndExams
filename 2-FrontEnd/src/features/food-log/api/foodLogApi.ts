import { apiClient } from '../../../core/api/client';
import type { DailyLog, DailyLogItem, AddLogItemRequest, DailyPointsHistory, FoodItem } from '../types/foodLog.types';

export const foodLogApi = {
  getTodayLog: () =>
    apiClient.get<DailyLog>('/daily-logs/today').then((r) => r.data),

  getHistory: () =>
    apiClient.get<DailyPointsHistory[]>('/daily-logs/history').then((r) => r.data),

  addItem: (data: AddLogItemRequest) =>
    apiClient.post<DailyLogItem>('/daily-logs/today/items', data).then((r) => r.data),

  removeItem: (itemId: string) =>
    apiClient.delete(`/daily-logs/today/items/${itemId}`),

  getFoodItems: (search?: string) =>
    apiClient
      .get<FoodItem[]>('/food-items', { params: search ? { search } : undefined })
      .then((r) => r.data),
};

