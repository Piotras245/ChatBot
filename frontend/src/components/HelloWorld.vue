<!-- 
<template>
  <div class="container pb-28 dark:bg-gray-800 bg-[#194b43] min-h-screen">
    <div class="flex flex-1 w-full">
      <div class="grid w-full bg-[#194b43]">
        <div>
          <h1 class="mb-4 text-center text-3xl font-extrabold text-gray-900 dark:text-white md:text-4xl lg:text-5xl">
            <span class="text-transparent bg-clip-text bg-gradient-to-r to-[#8BDBC3] from-[#42B996]">Chat</span>
          </h1>
          <div class="text-center mt-10">
            <div class="flex items-start gap-2.5 justify-center bg-[#194b43]">
              <div class="flex flex-col w-full max-w-[500px] bg-[#194b43]">
                <div v-for="(msg, index) in answers" :key="index"
                  class="flex flex-col w-full max-w-[500px] leading-1.5 p-4 mt-4 border-gray-200 bg-[#3D7068] rounded-xl dark:bg-gray-700">
                  <div class="flex items-center space-x-2 rtl:space-x-reverse">
                    <span class="text-sm font-semibold text-sky-100 dark:text-white">Chat response</span>
                    <span class="text-sm font-normal text-sky-100 dark:text-gray-400">{{ msg.timestamp }}</span>
                  </div>
                  <p class="mt-2 text-sm font-normal py-2.5 text-sky-50 dark:text-white">{{ msg.text }}</p>
                </div>
                <div v-if="loading" class="text-sm text-[#B3F5E0] dark:text-gray-400 mt-2">
                  <progress class="progress w-56" style="background-color: #3AA887; color: #B3F5E0;"></progress>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="fixed bottom-0 w-full flex justify-center px-4 z-50 bg-[#194b43]">
        <div class="w-full max-w-2xl mb-4 border border-gray-200 rounded-xl ring-0 border-0 bg-[#A4DFCD] dark:bg-gray-700">
          <div class="py-2 bg-[#A4DFCD] rounded-xl dark:bg-gray-800 ring-0 border-0">
            <div class="grid grid-cols-8 gap-1 px-2 bg-[#A4DFCD] ring-0 border-0">
              <textarea v-model="input" @keydown.enter.prevent="askLlama"
                class="col-span-7 bg-[#A4DFCD] w-full max-h-[200px] px-2 py-2 text-sm text-[#133730] dark:bg-gray-800 focus:outline-none border-0 dark:text-white resize-none"
                placeholder="Ask a question..." required></textarea>
              <button @click="askLlama"
                class="group inline-flex items-center py-2.5 px-4 text-xs font-medium text-white rounded-lg ml-2 bg-[#A4DFCD] ring-0 border-0">
                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="#5F887C"
                  class="h-10 w-8 group-hover:fill-sky-100 cursor-pointer transition-colors">
                  <path d="M3 21L21 12L3 3V10L13.5 12L3 14V21Z" />
                </svg>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
    import { DxDataGrid } from "devextreme-vue/data-grid"; // Import the DevExtreme DataGrid component
import AssistantResponse  from '@/types/AssistantResponse';


const input = ref('')
const answers= ref<AssistantResponse[]>([])
const loading = ref(false)

const askLlama = async () => {
  if (!input.value.trim()) return

  loading.value = true

  try {
    const encodedInput = encodeURIComponent(input.value)
    const response = await fetch(`http://localhost:5132/Query?input=${encodedInput}`)

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`)
    }

    const responseText = await response.text()

    answers.value.push({
      text: responseText,
      timestamp: new Date().toLocaleTimeString()
    })

    input.value = ''
  } catch (error) {
    answers.value.push({
      text: 'error',
      timestamp: new Date().toLocaleTimeString()
    })
    console.error(error)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
/* Your styles can go here */
</style> -->
