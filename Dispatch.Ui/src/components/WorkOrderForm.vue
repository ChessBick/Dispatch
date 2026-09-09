<script setup>
import { reactive, ref } from 'vue'
import { createWorkOrder } from '@/api/workOrderApi'

const emit = defineEmits(['created', 'cancel'])

const form = reactive({ title: '', description: '', assignedTo: '', scheduledDate: '' })
const submitting = ref(false)
const error = ref('')

async function submit() {
  if (!form.title.trim()) {
    error.value = 'Title is required.'
    return
  }
  submitting.value = true
  error.value = ''
  try {
    const created = await createWorkOrder({
      title: form.title,
      description: form.description || null,
      assignedTo: form.assignedTo || null,
      scheduledDate: form.scheduledDate || null,
    })
    emit('created', created.id)
  } catch {
    error.value = 'Failed to create work order.'
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <form @submit.prevent="submit">
    <h2>New Work Order</h2>
    <p v-if="error" class="error">{{ error }}</p>

    <label>
      <span>Title</span>
      <input v-model="form.title" required placeholder="e.g. Replace HVAC filter" />
    </label>
    <label>
      <span>Description</span>
      <textarea v-model="form.description" rows="3" placeholder="Optional details"></textarea>
    </label>
    <label>
      <span>Assigned To</span>
      <input v-model="form.assignedTo" placeholder="Technician name" />
    </label>
    <label>
      <span>Scheduled Date</span>
      <input type="date" v-model="form.scheduledDate" />
    </label>

    <div class="actions">
      <button type="submit" class="primary" :disabled="submitting">
        {{ submitting ? 'Creating…' : 'Create Work Order' }}
      </button>
      <button type="button" @click="emit('cancel')">Cancel</button>
    </div>
  </form>
</template>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  max-width: 480px;
}
label {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
  font-size: 0.85rem;
  color: var(--color-muted);
}
.actions {
  display: flex;
  gap: 0.5rem;
  margin-top: 0.5rem;
}
.error {
  color: var(--color-danger);
  background: var(--color-danger-bg);
  padding: 0.5rem 0.75rem;
  border-radius: var(--radius);
}
</style>