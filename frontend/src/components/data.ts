import CustomStore from "devextreme/data/custom_store";
import DataSource from "devextreme/data/data_source";
import { type ChatMessage } from "@/types/AssistantResponse";

export const dictionary = {
  en: {
    'dxChat-emptyListMessage': 'Chat is Empty',
    'dxChat-emptyListPrompt': 'AI Assistant is ready to answer your questions.',
    'dxChat-textareaPlaceholder': 'Ask AI Assistant...',
  },
}


export const REGENERATION_TEXT = 'Regeneration...';
export const CHAT_DISABLED_CLASS = 'dx-chat-disabled';
export const ALERT_TIMEOUT = 1000 * 60;

export const user = {
  id: 'user',
};

export const assistant = {
  id: 'assistant',
  name: 'Virtual Assistant',
};

export const store:ChatMessage[] = [];
export const messages : string[] = [];

const customStore = new CustomStore<ChatMessage, number>({
  key: 'id',
  load: () => {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve([...store]);
      }, 0);
    });
  },
  insert: (message: ChatMessage) => {
    return new Promise((resolve) => {
      setTimeout(() => {
        store.push(message);
        resolve(message);
      }, 0);
    });
  },
});


export const dataSource = new DataSource({
  store: customStore,
  paginate: false,
})

