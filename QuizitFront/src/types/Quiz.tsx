import {Question} from "./Question";

export interface Quiz {
  QuizId: number;
  Title: string;
  Description?: string;
  ImageUrl?: string;
  CategoryName: string;
  CategoryId: number;
  UserName: string;
  UserId: number;
  Questions: Question[];
}