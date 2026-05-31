import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { foodLogApi } from '../api/foodLogApi';
import type { AddLogItemRequest } from '../types/foodLog.types';

export const FOOD_LOG_KEYS = {
  today:    ['food-log', 'today']   as const,
  history:  ['food-log', 'history'] as const,
  items:    (search?: string) => ['food-items', search] as const,
};

export function useTodayLog() {
  return useQuery({
    queryKey: FOOD_LOG_KEYS.today,
    queryFn:  foodLogApi.getTodayLog,
  });
}

export function useFoodItems(search?: string) {
  return useQuery({
    queryKey: FOOD_LOG_KEYS.items(search),
    queryFn:  () => foodLogApi.getFoodItems(search),
  });
}

export function useAddLogItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: AddLogItemRequest) => foodLogApi.addItem(data),
    onSuccess: () => {
      void qc.invalidateQueries({ queryKey: FOOD_LOG_KEYS.today });
    },
  });
}

export function useRemoveLogItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (itemId: string) => foodLogApi.removeItem(itemId),
    onSuccess: () => {
      void qc.invalidateQueries({ queryKey: FOOD_LOG_KEYS.today });
    },
  });
}

export function useFoodLogHistory() {
  return useQuery({
    queryKey: FOOD_LOG_KEYS.history,
    queryFn:  foodLogApi.getHistory,
  });
}
