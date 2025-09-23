import axios from "axios";
import { Checkmark, CreateCheckmarkRequest, UpdateCheckmarkRequest, PriorityLevel } from "../models/Checkmark";

const API_BASE_URL = "http://localhost:5199/api";

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  console.log('Enviando request:', {
    url: config.url,
    method: config.method,
    data: config.data
  });
  return config;
});

api.interceptors.response.use(
  (response) => {
    console.log('Resposta recebida:', {
      url: response.config.url,
      status: response.status,
      data: response.data
    });
    return response;
  },
  (error) => {
    console.error('Erro na resposta:', {
      url: error.config?.url,
      status: error.response?.status,
      data: error.response?.data,
      message: error.message
    });
    return Promise.reject(error);
  }
);

export const checkmarkApi = {
  getAll: async (): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>("/checkmark");
    return response.data;
  },

  getById: async (id: number): Promise<Checkmark> => {
    const response = await api.get<Checkmark>(`/checkmark/${id}`);
    return response.data;
  },

  create: async (checkmark: CreateCheckmarkRequest): Promise<Checkmark> => {
    const payload = {
      ...checkmark,
      priority: Number(checkmark.priority), 
      dueDate: checkmark.dueDate || null,
      description: checkmark.description || ""
    };
    
    const response = await api.post<Checkmark>("/checkmark", payload);
    return response.data;
  },

  update: async (id: number, checkmark: UpdateCheckmarkRequest): Promise<void> => {
    const payload = {
      ...checkmark,
      priority: Number(checkmark.priority), 
      dueDate: checkmark.dueDate || null,
      description: checkmark.description || ""
    };
    
    await api.put(`/checkmark/${id}`, payload);
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/checkmark/${id}`);
  },

  getCompleted: async (): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>("/checkmark/completed");
    return response.data;
  },

  getPending: async (): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>("/checkmark/pending");
    return response.data;
  },

  getByPriority: async (priority: PriorityLevel): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>(`/checkmark/priority/${priority}`);
    return response.data;
  },
};