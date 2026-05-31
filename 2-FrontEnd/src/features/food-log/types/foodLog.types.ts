export interface FoodItem {
  id: string;
  name: string;
  servingSize: string;
  points: number;
  category: string;
}

export interface DailyLogItem {
  id: string;
  foodItemId: string;
  foodItemName: string;
  category: string;
  quantity: number;
  pointsComputed: number;
  mealTime: string;
}

export interface DailyLog {
  id: string;
  logDate: string;
  totalPoints: number;
  items: DailyLogItem[];
}

export interface AddLogItemRequest {
  foodItemId: string;
  quantity: number;
  mealTime: string;
}

export interface DailyPointsHistory {
  userId: string;
  fullName: string;
  logDate: string;
  totalPoints: number;
  foodItemCount: number;
}
