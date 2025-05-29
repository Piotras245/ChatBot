<template>
  <div class="container h-screen">
    <h1 class="mb-4 text-center font-extrabold text-gray-900 dark:text-white top-0 md:text-4xl lg: text-8xl">
      <span class="text-transparent bg-clip-text bg-gradient-to-r to-[#8BDBC3] from-[#42B996]">Chat Skibidi</span>
    </h1>
    <div class="grid grid-cols-6 gap-4 justify-center h-[90%]">
      <div class="col-span-1 px-4 justify-between space-y-5">
        <button @click=createNewChat()
          class="cursor-pointer w-full mb-3 lg:text-lg md:text-sm inline-flex items-center justify-center p-0.5 me-2 overflow-hidden font-medium text-[#42B996] hover:bg-neutral-100 rounded-lg ">
          Create new chat
        </button>

        <ul class="flex flex-col gap-2">
          <li v-for="chat in chatList" :key="chat.id" @contextmenu.prevent="openRenameMenu(chat.id, chat.name)" :class="[
            'flex items-center mt-2 text-sm text-gray-700 rounded-2xl p-2',
            currentChatId === chat.id ? 'bg-neutral-300 text-black' : 'hover:bg-neutral-100', 'mt-2'
          ]">
            <span @click="loadMessagesForChat(chat.id)" class="cursor-pointer w-full">
              <template v-if="renameChatId === chat.id">
                <input v-model="newChatName" @keyup.enter="submitRenameChat(chat.id)" @blur="cancelRename()"
                  autofocus />
              </template>
              <template v-else>
                {{ chat.name }}
              </template>
            </span>
            <svg @click="DeleteChat(chat.id)" class="group w-5 h-6 cursor-pointer" viewBox="0 -0.5 25 25"
              xmlns="http://www.w3.org/2000/svg">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <path class=" transition-colors duration-300 group-hover:fill-black " fill="#999999"
                  d="M6.96967 16.4697C6.67678 16.7626 6.67678 17.2374 6.96967 17.5303C7.26256 17.8232 7.73744 17.8232 8.03033 17.5303L6.96967 16.4697ZM13.0303 12.5303C13.3232 12.2374 13.3232 11.7626 13.0303 11.4697C12.7374 11.1768 12.2626 11.1768 11.9697 11.4697L13.0303 12.5303ZM11.9697 11.4697C11.6768 11.7626 11.6768 12.2374 11.9697 12.5303C12.2626 12.8232 12.7374 12.8232 13.0303 12.5303L11.9697 11.4697ZM18.0303 7.53033C18.3232 7.23744 18.3232 6.76256 18.0303 6.46967C17.7374 6.17678 17.2626 6.17678 16.9697 6.46967L18.0303 7.53033ZM13.0303 11.4697C12.7374 11.1768 12.2626 11.1768 11.9697 11.4697C11.6768 11.7626 11.6768 12.2374 11.9697 12.5303L13.0303 11.4697ZM16.9697 17.5303C17.2626 17.8232 17.7374 17.8232 18.0303 17.5303C18.3232 17.2374 18.3232 16.7626 18.0303 16.4697L16.9697 17.5303ZM11.9697 12.5303C12.2626 12.8232 12.7374 12.8232 13.0303 12.5303C13.3232 12.2374 13.3232 11.7626 13.0303 11.4697L11.9697 12.5303ZM8.03033 6.46967C7.73744 6.17678 7.26256 6.17678 6.96967 6.46967C6.67678 6.76256 6.67678 7.23744 6.96967 7.53033L8.03033 6.46967ZM8.03033 17.5303L13.0303 12.5303L11.9697 11.4697L6.96967 16.4697L8.03033 17.5303ZM13.0303 12.5303L18.0303 7.53033L16.9697 6.46967L11.9697 11.4697L13.0303 12.5303ZM11.9697 12.5303L16.9697 17.5303L18.0303 16.4697L13.0303 11.4697L11.9697 12.5303ZM13.0303 11.4697L8.03033 6.46967L6.96967 7.53033L11.9697 12.5303L13.0303 11.4697Z">
                </path>
              </g>
            </svg>
          </li>
        </ul>

      </div>
      <div class="col-span-5 chat-container w-[80%] h-full ">

        <DxChat :data-source="messages" :class="{ 'dx-chat-disabled': isDisabled }" :height="600"
          :reload-on-change="false" :show-avatar="false" :show-day-headers="false" :user="user" ref="assistantChat"
          message-template="message" v-model:typing-users="typingUsers" v-model:alerts="alerts"
          @message-entered="onMessageEntered">
          <template #message="{ data }">
            <div class="message-container w-100">
              <span v-if="data.message.text === REGENERATION_TEXT">
                {{ REGENERATION_TEXT }}
              </span>
              <template v-else>
                <div class="justify-between flex ">
                  <div v-html="convertToHtml(data.message.text)" class="dx-chat-messagebubble-text"></div>
                  <DxLoadIndicator :visible="loading" class="button-indicator" :height="20" :width="20" />
                </div>

                <div class="dx-bubble-button-container">
                  <DxButton :icon="copyButtonIcon" styling-mode="text" hint="Copy"
                    @click="onCopyButtonClick(data.message)" />

                </div>


              </template>
            </div>
          </template>
        </DxChat>

      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { ref, onBeforeMount, type Ref } from 'vue';
import DxChat, { type DxChatTypes } from 'devextreme-vue/chat';
import DxButton from 'devextreme-vue/button';
import { loadMessages } from 'devextreme/localization';
import type { chatFunct, ChatMessage } from "@/types/AssistantResponse";
import { DxLoadIndicator } from 'devextreme-vue/load-indicator';
import {
  dictionary,
  user,
  dataSource,
  REGENERATION_TEXT,
  assistant,
} from './data.ts';
import type ChatMessageDto from '@/types/ChatMessageDto.ts';
import { AuthorRole } from '@/types/AuthorRole.ts';


// import PulseLoader from 'vue-spinner/src/PulseLoader.vue'

const loading = ref(false);
const apiUrl = import.meta.env.VITE_API_URL;


const typingUsers = ref([]);
const alerts = ref([]);
const isDisabled = ref(false);
const copyButtonIcon = ref('copy');


function convertToHtml(text: string): string {
  return text.replace(/\n/g, '<br>');
}

onBeforeMount(() => {
  fetchChats()
  loadMessages(dictionary);
});

const messages: Ref<ChatMessage[]> = ref<ChatMessage[]>([])
const assistantChat: Ref<DxChat | undefined> = ref<DxChat>()

const currentChatId = ref<number | null>(null);

async function getAIResponse(promptText: string): Promise<string> {
  if (!promptText.trim()) return 'Prompt is empty.';
  if (!currentChatId.value) return 'No chat selected.';

  loading.value = true;

  try {
    const response = await fetch(
      `${apiUrl}Query?input=${encodeURIComponent(promptText)}&chatId=${currentChatId.value}&currentDateTime=${new Date().toLocaleTimeString()}`
    );

    if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

    const responseText = await response.text();

    return responseText;
  } catch (error) {
    console.error('AI Response error:', error);
    return 'An error occurred while fetching the response.';
  } finally {
    loading.value = false;
  }
}


async function processMessageSending(message: chatFunct, event?: Event) {
  toggleDisabledState(true, event);

  try {
    const aiResponse = await getAIResponse(message.text);

    setTimeout(() => {
      typingUsers.value = [];
      renderAssistantMessage(aiResponse);
    }, 200);
  } catch {
    typingUsers.value = [];
  } finally {
    toggleDisabledState(false, event);
  }
}

function toggleDisabledState(disabled: boolean, event?: Event) {
  isDisabled.value = disabled;

  if (disabled) {
    (event?.target as HTMLInputElement)?.blur();
  } else {
    (event?.target as HTMLInputElement)?.focus();
  }
}

function renderAssistantMessage(text: string) {
  const message = {
    id: Date.now(),
    timestamp: new Date().toLocaleTimeString(),
    author: assistant,
    text,
  };
  messages.value.push(message)
  assistantChat.value?.instance?.getDataSource().reload()

}


function onMessageEntered({ message, event }: DxChatTypes.MessageEnteredEvent) {

  assistantChat.value?.instance?.getDataSource().reload()

  if (!alerts.value.length && message) {
    processMessageSending(message as chatFunct, event);
  }
}


function onCopyButtonClick(message: { text: string }) {
  navigator.clipboard?.writeText(message.text);
  copyButtonIcon.value = 'check';

  setTimeout(() => {
    copyButtonIcon.value = 'copy';
  }, 1000);
}

const chatList = ref<{ id: number; name: string }[]>([]);

async function createNewChat() {
  try {
    const response = await fetch(`${apiUrl}Query/CreateChat`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      }
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const newChat = await response.json();

    chatList.value.unshift(newChat);

    await loadMessagesForChat(newChat.id);

    console.log("New chat created:", newChat);

  } catch (error) {
    console.error("Failed to create new chat:", error);
  }
}

async function fetchChats() {
  try {
    const response = await fetch(`${apiUrl}Query/AllChats`);

    console.log(apiUrl);

    if (!response.ok) {
      throw new Error(`Failed to fetch chats: ${response.status}`);
    }

    const data = await response.json();

    chatList.value = data;
    console.log(chatList);

    if (chatList.value.length === 0) {
      createNewChat();
    }

    const lastChatId = localStorage.getItem('lastChatId');
    const chatToLoad = lastChatId && chatList.value.some(chat => chat.id === +lastChatId)
      ? +lastChatId
      : chatList.value[0]?.id;
    if (chatToLoad) {
      await loadMessagesForChat(chatToLoad);
    }
  } catch (error) {
    console.error("Error fetching chats:", error);
  }
}

async function loadMessagesForChat(chatId: number) {
  if (loading.value) {
    console.warn('AI is responding, message loading is temporarily disabled.');
    return;
  }

  currentChatId.value = chatId;
  localStorage.setItem('lastChatId', chatId.toString());

  try {
    const response = await fetch(`${apiUrl}Query/MessagesByChat/${chatId}`);
    if (!response.ok) throw new Error(`Failed to load messages: ${response.status}`);

    const messagesTest = await response.json();

    dataSource.reload();

    messages.value = [];
    messagesTest.forEach((msg: ChatMessageDto) => {
      messages.value.push({
        id: msg.id,
        timestamp: msg.date,
        author: msg.role === AuthorRole.user ? user : assistant,
        text: msg.text,
      });
    });

    assistantChat.value?.instance?.getDataSource().reload();

  } catch (error) {
    console.error("Error loading messages:", error);
  }
}


async function DeleteChat(chatId: number) {
  const confirmed = window.confirm("Are you sure you want to delete this chat?");
  if (!confirmed) return;
  try {
    const response = await fetch(`${apiUrl}Query/DeleteChat/${chatId}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Failed to delete chat: ${response.status}`);
    }

    chatList.value = chatList.value.filter(chat => chat.id !== chatId);

    if (currentChatId.value === chatId) {
      currentChatId.value = null;
      dataSource.reload();
    }

    console.log(`Chat ${chatId} deleted successfully`);
  } catch (error) {
    console.error("Error deleting chat:", error);
  }
}

const renameChatId = ref<number | null>(null);
const newChatName = ref('');


function openRenameMenu(chatId: number, currentName: string) {
  renameChatId.value = chatId;
  newChatName.value = currentName;
}

function cancelRename() {
  renameChatId.value = null;
  newChatName.value = '';
}

async function submitRenameChat(chatId: number) {
  if (!newChatName.value.trim()) return;

  try {
    const response = await fetch(`${apiUrl}Query/RenameChat/${chatId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ newName: newChatName.value.trim() }),
    });

    if (!response.ok) throw new Error('Failed to rename chat.');

    const updatedChat = await response.json();

    const chatIndex = chatList.value.findIndex(c => c.id === chatId);
    if (chatIndex !== -1) {
      chatList.value[chatIndex].name = updatedChat.name;
    }

    renameChatId.value = null;
    newChatName.value = '';

  } catch (error) {
    console.error(error);
  }
}


</script>


<style>
#app {
  background-color: white;
  height: 100%;
}
</style>