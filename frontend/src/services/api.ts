import axios from "axios";
import { Checkmark, CreateCheckmarkRequest, UpdateCheckmarkRequest, PriorityLevel } from "../models/Checkmark";


const API_BASE_URL = "http://localhost:3000/api";

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const checkmarkApi = {
  // GET ALL
  getAll: async (): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>("/checkmark");
    return response.data;
  },

  // GET BY ID
  getById: async (id: number): Promise<Checkmark> => {
    const response = await api.get<Checkmark>(`/checkmark/${id}`);
    return response.data;
  },

  // POST
  create: async (checkmark: CreateCheckmarkRequest): Promise<Checkmark> => {
    const response = await api.post<Checkmark>("/checkmark", checkmark);
    return response.data;
  },

  // PUT
  update: async (id: number, checkmark: UpdateCheckmarkRequest): Promise<void> => {
    await api.put(`/checkmark/${id}`, checkmark);
  },

  // DELETE
  delete: async (id: number): Promise<void> => {
    await api.delete(`/checkmark/${id}`);
  },

  // GET COMPLETED
  getCompleted: async (): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>("/checkmark/completed");
    return response.data;
  },

  // GET PENDING
  getPending: async (): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>("/checkmark/pending");
    return response.data;
  },

  // GET BY PRIORITY
  getByPriority: async (priority: PriorityLevel): Promise<Checkmark[]> => {
    const response = await api.get<Checkmark[]>(`/checkmark/priority/${priority}`);
    return response.data;
  },
}
