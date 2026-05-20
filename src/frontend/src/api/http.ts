import axios from "axios";

export const http = axios.create({
  baseURL: "http://localhost:5136/api/v1",
});