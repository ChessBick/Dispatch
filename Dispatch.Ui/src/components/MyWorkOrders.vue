<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { listWorkOrders } from '@/api/workOrderApi'

const emit = defineEmits(['select'])

const technician = ref(localStorage.getItem('dispatch.technician') ?? '')
const statusFilter = ref('')
const statusOptions = ['Pending', 'InProgress', 'Done', 'Canceled']
const workOrders = ref([])
const loading = ref(false)
const error = ref('')

const myWorkOrders = computed(() => {
  const name = technician.value.trim().toLowerCase()
  if (!name) return []
  return workOrders.value.filter(wo => (wo.assignedTo ?? '').toLowerCase() === name)
})

async function load() {
  loading.value = true
  error.value = ''
  try {
    const result = await listWorkOrders({ status: statusFilter.value })
    workOrders.value = result.items
  } catch {
    error.value = 'Failed to load work orders.'
  } finally {
    loading.value = false
  }
}

watch(statusFilter, load)
watch(technician, (name) => localStorage.setItem('dispatch.technician', name))
onMounted(load)
</script>

<template>
  <div>
    <div class="toolbar">
      <input v-model="technician" placeholder="Your name (e.g. Jordan)" />
      <select v-model="statusFilter">
        <option value="">All statuses</option>
        <option v-for="s in statusOptions" :key="s" :value="s">{{ s }}</option>
      </select>
    </div>

    <p v-if="loading" class="muted">Loading...</p>
    <p v-else-if="error" class="error">{{ error }}</p>
    <p v-else-if="!technician" class="muted">Enter your name to see your assigned work orders.</p>

    <ul v-else-if="myWorkOrders.length" class="wo-list">
      <li v-for="wo in myWorkOrders" :key="wo.id" @click="emit('select', wo.id)">
        <div>
          <strong>{{ wo.title }}</strong>
          <p class="muted">{{ wo.scheduledDate ? new Date(wo.scheduledDate).toLocaleDateString() : 'Unscheduled' }}</p>
        </div>
        <span class="badge" :class="wo.status">{{ wo.status }}</span>
      </li>
    </ul>

    <p v-else class="muted">No work orders assigned to you.</p>
  </div>
</template>

<style scoped>
.toolbar { display: flex; gap: 0.5rem; margin-bottom: 1rem; }
.wo-list { list-style: none; padding: 0; margin: 0; }
.wo-list li {
  display: flex; justify-content: space-between; align-items: center;
  padding: 0.75rem; border-bottom: 1px solid var(--color-border); cursor: pointer;
}
.wo-list li:hover { background: var(--color-bg); }
.badge {
  padding: 0.2rem 0.6rem; border-radius: 999px; font-size: 0.75rem; font-weight: 600;
}
.badge.Pending { background: #fef3c7; color: #92400e; }
.badge.InProgress { background: #dbeafe; color: #1e40af; }
.badge.Done { background: #dcfce7; color: #166534; }
.badge.Canceled { background: #fee2e2; color: #991b1b; }
.muted { color: var(--color-muted); }
.error { color: var(--color-danger); }
</style>