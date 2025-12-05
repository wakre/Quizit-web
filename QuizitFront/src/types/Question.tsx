import { Answer } from "./Answer";

export interface Question {
  QuestionId: number;
  Text: string;
  Answers: Answer[];
  UserId: number;
}
