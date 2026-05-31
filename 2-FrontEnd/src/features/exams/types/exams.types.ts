export interface ExamCategory {
  id: string;
  name: string;
  sortOrder: number;
}

export interface Exam {
  id: string;
  name: string;
  abbreviation?: string;
  description?: string;
  categoryId: string;
  categoryName: string;
}

export interface ExamRequestItem {
  id: string;
  examId: string;
  examName: string;
  abbreviation?: string;
  examCategory: string;
  isCompleted: boolean;
  completedDate?: string;
  result?: string;
  laboratory?: string;
}

export interface ExamRequest {
  id: string;
  requestDate: string;
  doctorName: string;
  notes?: string;
  items: ExamRequestItem[];
}

export interface CreateExamRequestPayload {
  doctorName: string;
  notes?: string;
  examIds: string[];
}

export interface UpdateExamItemPayload {
  isCompleted: boolean;
  completedDate?: string;
  result?: string;
  laboratory?: string;
}
