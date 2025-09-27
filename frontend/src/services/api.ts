import { User } from "@/interfaces/User";
import axios from "axios";

const API_URL = "http://localhost:5055/api";

export const getUser = async (name: string): Promise<User> => {
  const { data } = await axios.get<User>(`${API_URL}/Users/${name}`);
  return data;
};

export const getSavedGames = async (): Promise<User[]> => {
  const { data } = await axios.get<User[]>(`${API_URL}/Users/saved-games`);
  return data;
};

export const saveUser = async (user: {
  name: string;
  balance: number;
}): Promise<User> => {
  const { data } = await axios.post<User>(`${API_URL}/Users/save`, user);
  return data;
};
