import { BetResult } from "./BetResult";

export interface Bet {
  betType: string;
  betColor: string;

  number: number;
  amount: number;
  result: BetResult;
}
