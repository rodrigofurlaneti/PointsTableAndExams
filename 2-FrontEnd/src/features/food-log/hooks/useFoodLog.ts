import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { foodLogApi } from '../api/foodLogApi';
import { useAuthStore } from '../../../core/auth/authStore';
import type { AddLogItemRequest } from '../types/foodLog.types';

const today = () => new Date().toISOString().split('T')[0];

export const FOOD_LOG_KEYS = {
  today: (userId?: string) => ['food-log', 'today', userId] as const,
  items: (search?: string) => ['food-items', search] as const,
};

export function useTodayLog() {
  const userId = useAuthStore(s => s.user?.id);
  return useQuery({
    queryKey: FOOD_LOG_KEYS.today(userId),
    queryFn: foodLogApi.getTodayLog,
    enabled: !!userId,
  });
}

export function useFoodItems(search?: string) {
  return useQuery({
    queryKey: FOOD_LOG_KEYS.items(search),
    queryFn: () => foodLogApi.getFoodItems(search),
  });
}

export function useAddLogItem() {
  const userId = useAuthStore(s => s.user?.id);
  const qc = useQueryClient();

  return useMutation({
    mutationFn: async (data: AddLogItemRequest) => {
      // Ensure today's log exists (creates if missing)
      const log = await foodLogApi.ensureTodayLog();
      return foodLogApi.addItem(log.id, data);
    },
    onSuccess: () => {
      void qc.invalidateQueries({ queryKey: FOOD_LOG_KEYS.today(userId) });
    },
  });
}
