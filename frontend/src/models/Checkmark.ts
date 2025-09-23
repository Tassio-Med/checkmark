export interface Checkmark {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate?: string
  priority: PriorityLevel;
  createdAt: string;
  updatedAt?: string;
}

export enum PriorityLevel {
  Low = 0,
  Medium = 1,
  High = 2,
}

export interface CreateCheckmarkRequest {
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate?: string;
  priority: PriorityLevel;
}

export interface UpdateCheckmarkRequest extends CreateCheckmarkRequest {
  id: number
  isCompleted: boolean;
}