import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { examsApi } from '../api/examsApi';
import type { CreateExamRequestPayload, UpdateExamItemPayload } from '../types/exams.types';

const KEYS = {
  categories:  ['exams', 'categories']   as const,
  all:         ['exams', 'all']          as const,
  requests:    ['exam-requests', 'me']   as const,
};

export function useExamCategories() {
  return useQuery({ queryKey: KEYS.categories, queryFn: examsApi.getCategories });
}

export function useAllExams() {
  return useQuery({ queryKey: KEYS.all, queryFn: examsApi.getExams });
}

export function useMyExamRequests() {
  return useQuery({ queryKey: KEYS.requests, queryFn: examsApi.getMyRequests });
}

export function useCreateExamRequest() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateExamRequestPayload) => examsApi.createRequest(data),
    onSuccess: () => void qc.invalidateQueries({ queryKey: KEYS.requests }),
  });
}

export function useUpdateExamItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ requestId, itemId, data }: { requestId: string; itemId: string; data: UpdateExamItemPayload }) =>
      examsApi.updateItem(requestId, itemId, data),
    onSuccess: () => void qc.invalidateQueries({ queryKey: KEYS.requests }),
  });
}
