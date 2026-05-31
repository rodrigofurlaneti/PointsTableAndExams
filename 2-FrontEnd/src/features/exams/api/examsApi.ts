import { apiClient } from '../../../core/api/client';
import type {
  Exam, ExamRequest, ExamCategory,
  CreateExamRequestPayload, UpdateExamItemPayload,
} from '../types/exams.types';

export const examsApi = {
  getCategories: () =>
    apiClient.get<ExamCategory[]>('/exams/categories').then((r) => r.data),

  getExams: () =>
    apiClient.get<Exam[]>('/exams').then((r) => r.data),

  getMyRequests: () =>
    apiClient.get<ExamRequest[]>('/exam-requests/me').then((r) => r.data),

  createRequest: (data: CreateExamRequestPayload) =>
    apiClient.post<ExamRequest>('/exam-requests', data).then((r) => r.data),

  updateItem: (requestId: string, itemId: string, data: UpdateExamItemPayload) =>
    apiClient.patch(`/exam-requests/${requestId}/items/${itemId}`, data).then((r) => r.data),
};
