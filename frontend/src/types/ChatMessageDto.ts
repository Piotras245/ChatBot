import type { AuthorRole } from './AuthorRole'

export default interface ChatMessageDto {
  id: number
  chatId: number
  role: AuthorRole
  text: string
  date: string
  timestamp: Date
}
