<script setup>
import { ref, onMounted, watch } from 'vue'
import { listWorkOrders } from '@/api/workOrderApi'

const emit = defineEmits(['select', 'create'])

const statusOptions = ['Pending', 'InProgress', 'Done', 'Canceled']
const statusFilter = ref('')
const workOrders = ref([])
const loading = ref(false)
const error = ref('')

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
onMounted(load)
defineExpose({ load })
</script>

<template>
  <div>
    <div class="toolbar">
      <select v-model="statusFilter">
        <option value="">All statuses</option>
        <option v-for="s in statusOptions" :key="s" :value="s">{{ s }}</option>
      </select>
      <button class="primary" @click="emit('create')">+ New Work Order</button>
    </div>

    <p v-if="loading" class="muted">Loading...</p>
    <p v-else-if="error" class="error">{{ error }}</p>

    <table v-else-if="workOrders.length">
      <thead>
        <tr><th>ID</th><th>Title</th><th>Status</th><th>Assigned To</th><th>Scheduled</th></tr>
      </thead>
      <tbody>
        <tr v-for="wo in workOrders" :key="wo.id" @click="emit('select', wo.id)">
          <td>#{{ wo.id }}</td>
          <td>{{ wo.title }}</td>
          <td><span class="badge" :class="wo.status">{{ wo.status }}</span></td>
          <td>{{ wo.assignedTo || '—' }}</td>
          <td>{{ wo.scheduledDate ? new Date(wo.scheduledDate).toLocaleDateString() : '—' }}</td>
        </tr>
      </tbody>
    </table>

    <p v-else class="muted">No work orders found.</p>
  </div>
</template>

<style scoped>
.toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.25rem;
}

table { width: 100%; border-collapse: collapse; }
th {
  text-align: left;
  padding: 0.6rem 0.75rem;
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.03em;
  color: var(--color-muted);
  border-bottom: 2px solid var(--color-border);
}
td {
  padding: 0.75rem;
  border-bottom: 1px solid var(--color-border);
}
tbody tr { cursor: pointer; }
tbody tr:hover { background: var(--color-bg); }

.badge {
  display: inline-block;
  padding: 0.2rem 0.6rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
}
.badge.Pending { background: #fef3c7; color: #92400e; }
.badge.InProgress { background: #dbeafe; color: #1e40af; }
.badge.Done { background: #dcfce7; color: #166534; }
.badge.Canceled { background: #fee2e2; color: #991b1b; }

.muted { color: var(--color-muted); }
.error { color: var(--color-danger); }
</style>