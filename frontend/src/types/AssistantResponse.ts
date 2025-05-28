export interface AssistantResponse {
  text: string;
  answers: string;
  loading: boolean;
  timestamp: string;
}

export interface ChatMessage {
  id: number;
  timestamp: string;
  author: {
    id: string;
    name?: string;
  };
  text: string;
}

export interface chatFunct {
  answers: string;
  text: string;
  timestamp: string;
  loading: boolean;
  message: string;
  // event?: any;
}
