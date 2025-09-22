export interface Checkmark {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate?: string
  priority: PriorityLevel;
  createdAt: string;
  updatedAt: string;
}

export enum PriorityLevel {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
}

export interface CreateCheckmarkRequest {
  title: string;
  description: string;
  dueDate?: string;
  priority: PriorityLevel;
}

export interface UpdateCheckmarkRequest extends CreateCheckmarkRequest {
  id: number
  isCompleted: boolean;
}